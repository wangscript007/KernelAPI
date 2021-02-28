You can customize the throttled response using the `QuotaExceededResponse` property of the `IpRateLimiting` or `ClientRateLimiting` configuration sections:

```json
"IpRateLimiting": {

    "QuotaExceededResponse": {
      "Content": "{{ \"message\": \"Whoa! Calm down, cowboy!\", \"details\": \"Quota exceeded. Maximum allowed: {0} per {1}. Please try again in {2} second(s).\" }}",
      "ContentType": "application/json",
      "StatusCode": 429
    },

}
```

- {0} - rule.Limit
- {1} - rule.Period
- {2} - retryAfter