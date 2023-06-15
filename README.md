# Import from Tempo to Toggl - v 1.1.0

To use the script you need to update file toggluserinformation.json and download the raw data from Tempo, copying its content on rawtempo.csv


## Authenticator

There is two ways you can use to identify your Toggl account:

1. Using your email and password
2. Using Toggl Token


### Email and password

To use email and password as authenticator, on toggluserinformation.json file:
- usertoken as a empty string
- useremail the email you use to login into Toggl
- userpassword the password you use to login into Toggl

Example:
```
{
    "usertoken":"",
    "useremail":"myemail@email.com",
    "userpassword":"mypassword",
    "workspaceid":123456
}
```

### Token

To use Toggl Token as authenticator, on toggluserinformation.json file:
- usertoken as token string
- useremail as a empty string
- userpassword as a empty string

Example:
```
{
    "usertoken":"abcdefgh1234567890",
    "useremail":"",
    "userpassword":"",
    "workspaceid":123456
}
```
