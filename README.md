# AcquirerDynamicRouting
Acquirer dynamic routing deals with the dynamically routing the transaction coming on Acquirer via CC and DC.


Introduction:

The current document describes the dynamic acquirer routing project which deals with dynamically routing the transaction to the specific acquirer based on routing configuration, transaction success rate and mdr of the acquirer. 

There are two working mode of routing in the project :
Acquirer Based Routing
Mixed Routing  (Acquirer vs issuer based routing)
Benefits of this project: 
Minimization of  transaction failure and transaction cost


2. Algorithm: 
The principal algorithm behind the routing is  :
Acquirer Health Status= p1(1+p)-p2   
where
P:   Total Success Rate
p=no of successful transaction/total no of transaction
P1:  Current Success Rate(e.g. last 100 transactions)
p1=no of successful transaction /total no of transaction  {in last 100 transaction}
P2:  Continuous Failure Rate
p2=no of continuous failure/maximum allowed continuous failure

Note: Acquirer health status will be saved to db after multiplying with 10000. Same will apply for p,p1 and p2 values.




3. Description of the Routing Modes:
3.1 Only Acquirer based routing: (Routing Mode -1)
The algorithm maintains the health status of every acquirer on the basis of the above mentioned equation. 
Routing condition:
An acquire having maximum Health status ,if It is not blocked and lowest mdr will be given priority for the selection.  
3.2 Mixed routing (Acquirer + Acquirer Vs Issuer) (Routing Mode-2)
The algorithm maintains the health status of every acquirer vs issuer combination on the basis of the above mentioned equation and it also maintains the  health status of every acquirer independently. 
Routing condition:
When the dynamic routing api will be called issuer information will also be passed if known. If the issuer is specified then the routing will be based on the mean of acquirer overall health status and issuer vs acquirer health status. If the issuer is not known then the routing will be based on only acquirer overall health status.


4. Starvation condition: 
If any acquirer is  not getting picked for the transaction for a long time then it will be treated as in starvation condition.

 Low transaction cost handling:
Low  transaction cost handling is achieved by the starvation condition handling . If the acquirer in the api request has the lowest mdr and it is in starvation condition then this acquirer will be selected for the transaction ignoring the condition that it’s health status is not good. This will be called a biased transaction. Number of the biased transactions will be limited for the acquirer . If the biased transaction touches the maximum allowed biased transaction then the biased transaction will be done after a specific time again if the acquirer will be still in starvation condition.
Key point about starvation: 
1: Starvation will be enabled after an acquirer is idle for a long period of time(not getting picked for the txns) .
2: Transaction in case of starvation will be done only for the acquirer with lowest mdr.
3. Blocked acquirers will not be triggered for the starve transaction.
4. Limited no of transactions will be passed to the starving acquirer irrespective of gateway health status.




5.Blocked condition handling:
An acquirer will be treated as blocked if it continuously fails the transaction . If the continuous fail transaction increases above the maximum allowed continuous fail  then the acquirer will be blocked . Similarly it is done for the acquirer-vs-issuer. If any acquirer is giving continuous fail with a specific issuer then that acquirer will be blocked for this issuer for the transaction.
Acquirer will be unblocked after it passes a specific amount of time in the blocked state. When the acquirer is unblocked its total continuous fail will be set to zero that will result in making P2 factor 0 in the algorithm and Its health status will increase. 
Acquirer will be unblocked by Scheduler. Api request will treat the acquirer unblocked if it has passed the blocking timespan.
 


6. Acquirer sorting technique
Acquirer sorting  will be based on Fitness value. The fitness value for each of the acquirers will be calculated by the following formula Inspired by Genetic Algorithm.
For routing mode 1:
starvation->health status value(unblock acquirer will be given more priority)
For routing mode 2:                
starvation->(health status value acquirer overall+health status with issuer)/2 (unblock acquirer and unblock acquirer with the given issuer will be given more priority )
     



8. Service Architecture:
This service will run independently. The service will expose an API to get the preferred acquirer order for the transaction. 

Service Architecture For only Acquirer based routing 
The api will get the current health status of acquirers from the table PINE_PG_DYNAMIC_ACQUIRER_ROUTING_DETAILS_TBL. Similarly issuer vs acquirer health status will be fetched by the table
PINE_PG_DYNAMIC_ACQUIRER_ROUTING_ACQUIRER_VS_ISSUER_DETAILS_TBL
Api call request and response will be saved to the table 
PINE_PG_DYNAMIC_ACQUIRER_ROUTING_MERCHANT_REQUEST_DETAILS_TBL

A scheduler will be run in the background.  It will get the transaction details from the transaction table and will calculate the acquirer health status for all the acquirers on the basis of the fetched transaction.  It will update the current acquirer health status that will be used by API in the next request.PINE_PG_DYNAMIC_ACQUIRER_ROUTING_CURRENT_DETAILS_TBL ,
PINE_PG_DYNAMIC_ACQUIRER_ROUTING_ACQUIRER_VS_ISSUER_CURRENT_DETAILS_TBL
And the data of the table  PINE_PG_DYNAMIC_ACQUIRER_ROUTING_DETAILS_TBL, PINE_PG_DYNAMIC_ACQUIRER_ROUTING_ACQUIRER_VS_ISSUER_DETAILS_TBL. Will be updated. After the schedular completes the cycle it will make the entry to 
PINE_PG_DYNAMIC_ACQUIRER_ROUTING_SESSION_DETAILS_TBL.
Figure 2.0 represents the working details.



9. Api Details
The api will be called as
Url: api/v1/DynamicRouting/FetchAcquirerOrderNew
Request Type: post
Request
{
    "merchant_id": 122,
    "max_health_differnce_to_be_allowed_by_merchant": 20000,
    "acquirer_list": [
        2,
        3,
        8001,
        23
    ],
    "pine_pg_txn_id": 3456,
    "issuer_id": 1
}

Response{
{
    "response_code": 1,
    "acquirer_order": [8001, 3, 2,23],
    "response_message": "success"
 }
 
 
10. Routing configuration:
Max allowed continuous fail to block acquire
<add key="DYNAMIC_ACQUIRER_ROUTING_MAX_ALLOWED_CONTINUOUS_FAIL" value="5" />
Max allowed continuous fail to block issuer vs acquirer (Ideally it should be lower than max allowed continuous fail to block acquirer)
<add    key="DYNAMIC_ACQUIRER_ROUTING_MAX_ALLOWED_CONTINUOUS_FAIL_TO_BLOCK_ISSUER_VS_ACQUIRER" value="5"/>
Current transaction limit for acquirer
<add key="DYNAMIC_ACQUIRER_ROUTING_MAX_CURRENT_TXN_LIMIT" value="90" />
Current transaction limit for acquirer vs issuer   
<add key="DYNAMIC_ACQUIRER_ROUTING_MAX_CURRENT_TXN_LIMIT_FOR_ISSUER_VS_ACQUIRER" value="100"/>
Time span to unblock acquirer in milliseconds
<add key="DYNAMIC_ACQUIRER_ROUTING_TIME_SPAN_TO_UNBLOCK_ACQUIRER_IN_MILLISECONDS" value="5000"/>
Time span to unblock acquirer vs issuer in milliseconds
  <add key="DYNAMIC_ACQUIRER_ROUTING_TIME_SPAN_TO_UNBLOCK_ISSUER_VS_ACQUIRER_IN_MILLISECONDS" value="5000"/>
Routing mode: Routing mode will be set by two keys
<add key="SHOULD_START_DYNAMIC_ACQUIRER_ROUTING_SERVICE" value="1"/> (1 or 0)
 <add key="DYNAMIC_ACQUIRER_ROUTING_SHOULD_INCLUDE_ISSUER_VS_ACQUIRER_BASED_ROUTING" value="1"/> (1 or 0)
Routing mode 1: 
SHOULD_START_DYNAMIC_ACQUIRER_ROUTING_SERVICE=1
DYNAMIC_ACQUIRER_ROUTING_SHOULD_INCLUDE_ISSUER_VS_ACQUIRER_BASED_ROUTING=0
Routing mode 2:
SHOULD_START_DYNAMIC_ACQUIRER_ROUTING_SERVICE=0
DYNAMIC_ACQUIRER_ROUTING_SHOULD_INCLUDE_ISSUER_VS_ACQUIRER_BASED_ROUTING=1

Routing mode 3: (Default routing)
SHOULD_START_DYNAMIC_ACQUIRER_ROUTING_SERVICE=1
DYNAMIC_ACQUIRER_ROUTING_SHOULD_INCLUDE_ISSUER_VS_ACQUIRER_BASED_ROUTING=1
Starvation handling:
<add key="DYNAMIC_ACQUIRER_ROUTING_SHOULD_ENABLE_ACQUIRER_STARVATION_HANDLING" value="1"/> (1 or 0)
  <add key="DYNAMIC_ACQUIRER_ROUTING_NO_OF_TRANSACTIONS_FOR_ACQUIRER_STARVATION_HANDLING" value="5"/>
 <add key="DYNAMIC_ACQUIRER_ROUTING_TIME_IN_MILI_SECONDS_TO_TRIGGER_ACQUIRER_STARVATION_HANDLING" value="5000"/>

Block condition handling:
Enable acquirer block handling  
<add key="DYNAMIC_ACQUIRER_ROUTING_SHOULD_ENABLE_ACQUIRER_BLOCK_HANDLING_BY_API" value="1"/> (1 or 0)
Enable Acquirer vs issuer block handling
<add key="DYNAMIC_ACQUIRER_ROUTING_SHOULD_ENABLE_ISSUER_VS_ACQUIRER_BLOCK_HANDLING_BY_API" value="1"/>
  
11.Service configuration 
Scheduler time
<add key="DYNAMIC_ACQUIRER_ROUTING_SCHEDULAR_TIME" value="10000000" />
Maximum row to be fetched at once from transaction table for scheduler
<add key="DYNAMIC_ACQUIRER_ROUTING_MAXIMUM_TRANSACTIONS_TO_BE_PROCESSED" value="1000" />
Cache Service 
<add key="DYNAMIC_ACQUIRER_ROUTING_SHOULD_ENABLE_API_CACHE_SERVICE" value="1"/>
To enable cache service set the value to 1
Cache expiry time 
<add key="DYNAMIC_ACQUIRER_ROUTING_API_CACHE_EXPIRY_TIME_IN_MILLISECONDS" value="120000"/>
Production Mode    flag should be 1 in case of production mode may use 0 in testing mode for checking mixed routing.
<add key="DYNAMIC_ACQUIRER_ROUTING_ENABLE_PRODUCTION_ENVIRONMENT" value="1"/>





Addons 
12. Test Cases For Only Acquirer Based Routing (An overview of how algorithm works



Transaction was done by selecting randomly a acquirer from the acquirers present in table  1.0 .
Where the variable represent as:
i: transaction no
iAcquirerIdForTransaction=randomly selected acquirer  
status: transaction status(true for successful txn and false for unsuccessful txn) 
Test 1: 
Transaction was marked alternatively true or false for all acquirers
Biased condition:  Transaction of acquirer 3 was biased as

               if(i>150&&i<180&& iAcquirerIdForTransaction==3)
                {
                    status= false;
                }
                if(i > 200 && i < 230 && iAcquirerIdForTransaction == 3)
                {
                    status= true;
                }
                if(i>250&& iAcquirerIdForTransaction==0)
                {
                    status= true;
                }

Test 2:
Transaction was marked alternatively true or false for all acquirers
Biased condition:  Randomly select an acquirer and mark  it transaction unsuccessful for 100 transaction



Test 3:

Transaction was marked alternatively true or false for all acquirers
Biased condition:  Randomly select an acquirer and mark  it transaction unsuccessful for 50 transactions 


Case 2:
New Acquirer Details



Test1: 
Transaction was marked alternatively true for all acquirers
Biased condition:  Randomly select an acquirer and mark  it transaction unsuccessful for 30 transactions 
Setting maximum continuous failure=30


Test1: 
Transaction was marked alternatively true for all acquirers
Biased condition:  All transactions was marked true for acquirer 3 except 
if (i%100>80&&i%100<99)
                {
                    status= false;
                }

Fig a:Setting maximum continuous failure=30
Fig b: Setting maximum continuous failure=5




Test3:
Transaction was marked alternatively true for all acquirers
Biased condition:  All transactions was marked true for acquirer 3 except 
if (i%100>70&&i%100<99)
                {
                    status= false;
                }

Fig a:Setting maximum continuous failure=20
Fig b: Setting maximum continuous failure=10

Test 4:
Transaction was marked alternatively true for all acquirers
Biased condition:  Randomly select an acquirer and mark  it transaction unsuccessful for 100 transaction
Fig a) Maximum allowed continuous failure=20
Fig b) Maximum allowed continuous failure=10
Fig a) Maximum allowed continuous failure=5





