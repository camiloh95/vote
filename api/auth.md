---
description: Here you will find different endpoints for the authentication
---

# Authentication

{% api-method method="post" host="https://{{URL}}" path="/auth/login" %}
{% api-method-summary %}
Login
{% endapi-method-summary %}

{% api-method-description %}
To get an access token and user's information
{% endapi-method-description %}

{% api-method-spec %}
{% api-method-request %}
{% api-method-body-parameters %}
{% api-method-parameter name="password" type="string" required=true %}
User's password
{% endapi-method-parameter %}

{% api-method-parameter name="email" type="string" required=true %}
User's email
{% endapi-method-parameter %}
{% endapi-method-body-parameters %}
{% endapi-method-request %}

{% api-method-response %}
{% api-method-response-example httpCode=200 %}
{% api-method-response-example-description %}
logged in successfully  
{% endapi-method-response-example-description %}

```
    "data": {
        "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpX",
        "scope": "load:read",
        "expires_in": 86400,
        "moduleTime": 857,
        "user_id": "randomId1234",
        "name": "John Testing",
        "carrier_id": "IDTEST",
        "auth0_id": "auth0|5ed94a7c123e3123123fb123",
        "email": "chernandez@edgelogistics.com",
        "type": "user",
        "timezone": "America/Chicago",
        "timezone_offset": -5,
        "status": "A",
        "phone_number": "1234567890",
        "avatar": {
            "type": "Buffer",
            "data": []
        }
    },
    "status": 200,
    "message": "logged in succesfully"
```
{% endapi-method-response-example %}

{% api-method-response-example httpCode=403 %}
{% api-method-response-example-description %}
Wrong email or password  
{% endapi-method-response-example-description %}

```
{
    "message": "Wrong email or password.",
    "code": "invalid_grant",
    "status": 403,
    "error": true
}
```
{% endapi-method-response-example %}

{% api-method-response-example httpCode=429 %}
{% api-method-response-example-description %}
Wrong email or password  
{% endapi-method-response-example-description %}

```
{
    "message": "Your account has been blocked",
    "code": "too_many_attempts",
    "status": 429,
    "error": true
}
```
{% endapi-method-response-example %}
{% endapi-method-response %}
{% endapi-method-spec %}
{% endapi-method %}

{% api-method method="post" host="https://{{URL}}" path="/auth/forgot-password" %}
{% api-method-summary %}
Forgot Password
{% endapi-method-summary %}

{% api-method-description %}
To send an email and recover the account
{% endapi-method-description %}

{% api-method-spec %}
{% api-method-request %}
{% api-method-body-parameters %}
{% api-method-parameter name="email" type="string" required=true %}
User's email
{% endapi-method-parameter %}
{% endapi-method-body-parameters %}
{% endapi-method-request %}

{% api-method-response %}
{% api-method-response-example httpCode=200 %}
{% api-method-response-example-description %}
Email sent successfully to reset the password
{% endapi-method-response-example-description %}

```
{
    "status": 200,
    "message": "We've just sent you an email"
}
```
{% endapi-method-response-example %}

{% api-method-response-example httpCode=400 %}
{% api-method-response-example-description %}
Email does not exist inside the database
{% endapi-method-response-example-description %}

```
{
    "message": "the email does not exist",
    "code": "EMAIL_DOES_NOT_EXIST",
    "status": 400,
    "error": tru
```
{% endapi-method-response-example %}
{% endapi-method-response %}
{% endapi-method-spec %}
{% endapi-method %}

