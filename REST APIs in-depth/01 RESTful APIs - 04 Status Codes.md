# RESTful APIs

## Status Codes

### What is a Status Code?

Status codes are three-digit codes that indicate the outcome of a request. They are part of the response and include information that helps the client know how to proceed.

There are 3 types of Status Codes that are commonly used: Successful (200s), Client Error (400s) and Server Error (500s).

### Commonly used Status Codes

| Type         | Code | Message                | Common use scenarios                                                                                                                  |
| ------------ | ---- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| Successful   | 200  | Ok                     | The request succeeded. Use when no further information can be provided with other successful status codes .                           |
| Successful   | 201  | Created                | A new resource was created.                                                                                                           |
| Successful   | 202  | Accepted               | A new resource was created but it has to be further processed.                                                                        |
| Successful   | 204  | No Content             | A resource was updated or deleted.                                                                                                    |
| Client Error | 400  | Bad Request            | The client sent an invalid request, e.g. bad data was submitted in a form.                                                            |
| Client Error | 401  | Unauthorized           | The client is not authenticated, e.g. the authorization header is missing or the client's JWT has expired.                            |
| Client Error | 403  | Forbidden              | The client is authenticated but not authorized, e.g. a specific permission, role or scope is not granted.                             |
| Client Error | 404  | Not Found              | The resource does not exist, e.g. trying to request a deleted record or an incorrect endpoint.                                        |
| Client Error | 405  | Method Not Allowed     | The endpoint is supported but the method isn't, e.g. the `GET /users` endpoint exists but the `POST /users` doesn't.                  |
| Client Error | 409  | Conflict               | The resource's state doesn't allow this operation, e.g. trying to create a duplicate resource.                                        |
| Client Error | 415  | Unsupported Media Type | The request is using an incorrect format, e.g. trying to post JSON to an endpoint that accepts file uploads.                          |
| Server Error | 500  | Internal Server Error  | The server encountered an unexpected error that was not handled properly. This is typically caused by bugs and/or lack of resiliency. |
| Server Error | 503  | Service Unavailable    | The server is down, e.g. maintenance, downtime caused by a deployment, overload or resource exhaustion.                               |
| Server Error | 504  | Gateway Timeout        | The request took too long to be processed and it timed out, e.g. issues with networking, proxies, load balancing, etc..               |

### References

- <https://blog.postman.com/what-are-http-status-codes/>
- <https://datatracker.ietf.org/doc/html/rfc7231#section-6.1>
- <https://datatracker.ietf.org/doc/html/rfc7235#section-3>

### Other Sections

- Prev: [Web Resources](./01%20RESTful%20APIs%20-%2003%20Web%20Resources.md)
- Next: [Versioning](./01%20RESTful%20APIs%20-%2005%20Versioning.md)
