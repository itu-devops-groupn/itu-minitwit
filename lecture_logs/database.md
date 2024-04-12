# Creating indexes
Looking at our monitoring dashboard we realised that our follower-endpoints were slow due to them not having an index.
We therefore decided to make an index on the who_id, which is the one that we primarily use in these endpoints.
To do this we connected to the database manually and executed:
```
CREATE INDEX idx_follower_whoid ON follower(who_id); 
``` 
We also had issues with loading times on the Public-page that led us to create an index on the publication date (pub_date) as we order this whenever someone tries to retrieve messages. Doing this we reduced the load time from 1,5 seconds (that could rise if you spammed the site) to sub 10 ms. This is done with the following:
```
CREATE INDEX idx_messages_pub_date ON message(pub_date);
```
