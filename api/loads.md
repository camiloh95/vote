---
description: >-
  Here you will find all the endpoints related with the loads (shipments). Take
  into account you need to use always a Bearer token here.
---

# Loads

{% api-method method="get" host="https://{{URL}}" path="/load/read" %}
{% api-method-summary %}
Read
{% endapi-method-summary %}

{% api-method-description %}
To get loads \(shipments\)
{% endapi-method-description %}

{% api-method-spec %}
{% api-method-request %}
{% api-method-path-parameters %}
{% api-method-parameter name="pallet\_count" type="string" required=false %}
Number of pallets
{% endapi-method-parameter %}

{% api-method-parameter name="equipment\_type" type="string" required=false %}
Equipment type
{% endapi-method-parameter %}

{% api-method-parameter name="weight" type="string" required=false %}
Load weight
{% endapi-method-parameter %}

{% api-method-parameter name="distance" type="string" required=false %}
Distance in miles 
{% endapi-method-parameter %}

{% api-method-parameter name="load\_type" type="string" required=false %}
Load type
{% endapi-method-parameter %}
{% endapi-method-path-parameters %}

{% api-method-headers %}
{% api-method-parameter name="Content-Type" type="string" required=true %}
application/json
{% endapi-method-parameter %}

{% api-method-parameter name="Authorization" type="string" required=true %}
Bearer token
{% endapi-method-parameter %}
{% endapi-method-headers %}
{% endapi-method-request %}

{% api-method-response %}
{% api-method-response-example httpCode=200 %}
{% api-method-response-example-description %}
Success request
{% endapi-method-response-example-description %}

```
{
    "data": [
        {
            "request_id": "2661",
            "shipment_gid": "NBL.NB21744863",
            "load_type": "NBL.TRUCK",
            "distance": "183.7",
            "weight": "42978",
            "equipment_type": "VAN53",
            "pallet_count": "19",
            "stops": [
                {
                    "stop_sequence_number": "1",
                    "load_id": "NBL.NB21744863",
                    "stop_type": "P",
                    "customer_po": "121910",
                    "location_gid": "NBL.ORG-109-145",
                    "address_line": "2560 E PHILADELPHIA ST.",
                    "city": "ONTARIO",
                    "state": "CA",
                    "country": "USA",
                    "postal_code": "91761",
                    "appointment_date_time": "2030-12-20T09:15:00.000Z",
                    "appointment_tz": "America/Los_Angeles",
                    "distance": "0",
                    "distance_uom": "MI"
                }
            ]
        }
    ],
    "status": 200,
    "message": "OK"
}
```
{% endapi-method-response-example %}

{% api-method-response-example httpCode=401 %}
{% api-method-response-example-description %}
Without bearer token
{% endapi-method-response-example-description %}

```
{
    "message": "No authorization token was found",
    "code": "credentials_required",
    "status": 401,
    "error": true
}
```
{% endapi-method-response-example %}

{% api-method-response-example httpCode=403 %}
{% api-method-response-example-description %}
Your token does not have sufficient permissions
{% endapi-method-response-example-description %}

```
Insufficient scope
```
{% endapi-method-response-example %}
{% endapi-method-response %}
{% endapi-method-spec %}
{% endapi-method %}



