Solution is designed as N-tier layer architechture :-

![Screenshot 2024-09-30 083025](https://github.com/user-attachments/assets/2cc711c4-922a-4ba7-b927-70a5adcf9f2b)



Coffee machine stock is stored in a sql server database with extra info such as refilldate. On first deploy, table `CoffeeStocks` will be seeded with quantity 4 as full stock.
As people are making calls, the stock will go down. 

There is another `PUT` method created to refill the machine.
Assumption is server will be running on UTC time or not likely within same time zone as client. Created a `TimeZoneId` key value in `appsettings.json` depending on this timezoneid, 
`prepared` datetimeoffset will be calculated.

-------------------------------------

The `extra_credit` branch:- 
--------------------------------------
Created V2 call for `brew-coffee` which will get city temperature for the day from OpenWeather API.

openweather api is consumed. I created an API key from their documentation. Changed appsettings.json to have one object called `City` this will have timezoneid, City name is required to make API call openweather. Assumption :- the city information is not provided in the requirement, Whether client will send the city name or the service is running specific to city. 
So added in appsettings.json.

In order to avoid, making third party call on every single request. Local caching is introduced and the cache expiration is set to 3 hours (assumption for 3 hours city temperature will remain same.
This can be reduced to 2 hours may be. Idea is try to make third party calls as minimal as possible.

This local memory cache can be changed to utilise redis.
