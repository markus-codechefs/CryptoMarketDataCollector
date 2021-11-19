# CoinMarketCap API collector
Utilizes a azure function to updated the coinmarketcap.com prices from their API to a storage on azure. 
The function triggers every 8 hours - (the api is limited to the free tier)

Reason
The financial history of coinmarketcap is pay per use. With this data a separate program may track personal portoflio prices and changes.
