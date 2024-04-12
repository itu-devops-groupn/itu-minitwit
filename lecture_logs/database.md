# Creating indexes
Looking at our monitoring dashboard we realised that our follower-endpoints were slow due to them not having an index.
We therefore decided to make an index on the who_id, which is the one that we primarily use in these endpoints.
To do this we connected to the database manually and executed:
```
CREATE INDEX idx_follower_whoid ON follower(who_id); 
``` 
