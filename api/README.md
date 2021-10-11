
REST API description
===

- `/movie/like/{movie id}` (POST) likes a movie that has an ID of `movie id`.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.


- `/movie/dislike/{movie id}` (POST) dislikes a movie that has an ID of `movie id`.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.


- `/movie` (GET) gets the list of movies that are contained in movie library.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

  Query parameters:
  - `limit` (required) limits how many movies are returned.
  - `offset` (optional) sets movie list offset to enable pagination. If not given, set to zero.


- `/movie/recommendations` (POST) gets movie recommendations based on genres of user's liked movies.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

  Query parameters:
  - `limit` (required) limits how many movie recommendations are returned.
  - `offset` (optional) sets movie recommendation offset to enable pagination. If not given, set to zero.


- `/user/token/{user id}` (POST) generates an access token for user with an ID of `user id`.


- `/user/moviePreference` (POST) create a subset of given movie IDs that contain only the movies that user likes.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

  Request body:
  - JSON array of movie ID strings.


You can use `/user/token/{user id}` endpoint to generate an access token for user with an ID of `user id`. This will generate an access token without an expiration date. I have already generated an access token for _Dummy user_ with an ID of 1. Feel free to use it if you like: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJEdW1teSB1c2VyIiwidXNlcklkIjoiMSIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.XZ0FhaeawcbnKLsyOUbSSRSU8DkEB6Mf2PsfJ3NQatw
`
