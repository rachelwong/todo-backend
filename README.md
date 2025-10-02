### Todo backend for angular-test

* This should run in localhost on port 5053
* in memory database seeded with two items
* available endpoints include

  1. GET /api/todo
  2. POST /api/todo which takes a request payload of
     ```
      {
         "description": "some string text",
         "done": true // boolean value
       }
     ```
  4. DELETE /api/todo/:id
 
* CORS set to allow all for the purposes of a locally run test application
