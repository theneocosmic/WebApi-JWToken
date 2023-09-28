# WebApi-JWToken

Example project on how to generate JWToken in .Net
This project contains a Countries endpoint that you can consume by providing a bearer token.

   
To do this, use the authenticate endpoint, provide the user and password and this endpoint will return, in case of being a valid user, the token and it will also return a refresh token.

   
This refresh token will be used to generate a new token when it has expired. It will no longer be necessary to re-enter the username and password, the refresh token will be used for this.

    
Inside the DbScripts folder, there is the script for you to restore the database.

  
I hope it helps you. Enjoy!

   
<img src="https://github.com/theneocosmic/WebApi-JWToken/blob/master/WebApi-JWToken/assets/JWTManageAPI.png" alt="screenshot">
