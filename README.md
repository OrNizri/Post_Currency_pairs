# Post_Currency_pairs

INTRODUCTION
This API adding new reocrds of currencies pairs to DB.
The currency pairs chosen : "USDILS", "USDEUR", "USDJPY", "USDGGP"

The pairs' value where taken by connecting to currencylayer api (see https://currencylayer.com/).

Post Currency Pairs Controllers:
1. HttpWebRequest created in order to get each pair value.
/*
Response example :
{
  "success":true,
  "terms":"https:\/\/currencylayer.com\/terms",
  "privacy":"https:\/\/currencylayer.com\/privacy",
  "timestamp":1642666263,
  "source":"USD",
  "quotes":{
    "USDAED":3.672995,
    "USDAFN":104.874914,
    "USDALL":107.287788,
    "USDAMD":479.753878,.....}
    */
2. In order to get the pair value from the response I chose to take the Substring created after the pair value
/* the substring of USDAMD for example if the pair chosen is "USDAMD" the substring is : 479.753878,.....*/
3. RetunPairValue - gets the substring created at the previous step and returns the first float number 
/* the returned value will be: 479.753878*/
4. UpdateInTableAsync - creates connection to the db and updating the table's values by Stored Procedure - Proc_Set_Currency_Pairs_

