
## ExchangeRates API

This API provides exchange rates-data from the official European Central Bank API:
https://sdw-wsrest.ecb.europa.eu/help/

### Stack:

* .Net 6
* MongoDB


### Endpoints:
* `https://localhost:7153/api/token`

Basic Auth with hardcoded Username and Password is used for Authorization .


*  `https://localhost:7153/api/rates?parameters`

Where parameters are defined such as **startDate**=*value*&**endDate**=*value*&**apiKey**=*value*
* **startDate**- *yyyy-mm--dd* format
*  **endDate** - *yyyy-mm--dd* format
* **apikey** - response from the `/api/token` endpoint needs to be provided here

Body - provides source and target exchange rate values in json format, like:
```diff
{
"PLN":  "EUR"
}
```



### Example:

Gets the exchange rates from *2021-01-01* to *2021-01-12*. The provided apiKey is for testing only.



```diff
https://localhost:7153/api/rates?startDate=2021-01-01&endDate=2021-01-12&apiKey=rfIHsYPyD0IiOMoyILth7qhBnnGQlOghlFXChNgj6FJqPoJD6suyrrJlLKHCuHYA
```
