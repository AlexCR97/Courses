# RESTful APIs

## Web Resources

### What is a Resource?

The data exchanged via HTTP is called a _resource_. It can be text, HTML documents, images, JSON, or anything that can be represented in binary.

Each resource is located at a Uniform Resource Locator (URL) and is uniquely identified by a Uniform Resource Identifier (URI).

### URL

![URL format](url-format.png)

### HTTP methods

| Method | Description                               |
| ------ | ----------------------------------------- |
| GET    | Retrieves a single or multiple resources. |
| POST   | Creates a new resource.                   |
| PUT    | Updates an existing resource.             |
| DELETE | Deletes a resource.                       |

Other HTTP methods that are less frequently used are:

- POST as upsert: An upsert is a combination of a POST and a PUT. If the resource does not exist, it will create a new resource, but if it does exist, it will update that existing resource. Some APIs allow upserts using the POST method.
- PATCH: Similar to PUT, but updates only a specific set of fields on the resource.
- HEAD: Similar to GET, but without the response body.

### Resource naming conventions

Rules:

- Use nouns, not verbs.
- Use `kebab-case` or `camelCase` (some systems use `PascalCase`). DON'T use `snake_case`.
- Use plurals when you can have multiple instances of a resource.
- Use singular when your resource is a singleton.
- Represent ownership using "/" for hierarchy.
- Don't allow trailing "/".

Good examples:

```text
POST   /orders                                    Create a new order.
GET    /orders                                    Retrieve all of the orders.

GET    /orders/{orderId}                          Retrieve the order's details.
PUT    /orders/{orderId}                          Update the order.
DELETE /orders/{orderId}                          Deletes the order.

POST   /orders/{orderId}/line-items               Add a line item to the order.
GET    /orders/{orderId}/line-items               Retrieve the order's line items.
PUT    /orders/{orderId}/line-items/{lineItemId}  Modify a line item from the order.
DELETE /orders/{orderId}/line-items/{lineItemId}  Remove a line item from the order.

GET    /orders/{orderId}/status                   Retrieve the order's status.
POST   /orders/{orderId}/submission               Submit the order.
POST   /orders/{orderId}/cancellation             Cancel the order.
```

Bad examples:

```text
POST /orders/create
GET  /orders/list
POST /orders/list
GET  /order/{orderId}
GET  /orders/line-items/{orderId}
POST /orders/{orderId}/submit
POST /orders/{orderId}/cancel
```

### Content-Type

The Content-Type header is used to indicate the media type (also named mime type) of the resource.

Example:

```text
Content-Type: application/json
```

Commonly used content types:

| Content Type                      | MIME Type   | Description                      |
| --------------------------------- | ----------- | -------------------------------- |
| application/octet-stream          |             | Any kind of binary data          |
| application/json                  | .json       | JSON                             |
| application/x-www-form-urlencoded |             | Key-Value pairs                  |
| application/pdf                   | .pdf        | Portable Document Format (PDF)   |
| application/zip                   | .zip        | ZIP archive                      |
| image/jpeg                        | .jpg, .jpeg | JPEG image                       |
| image/png                         | .png        | PNG image                        |
| multipart/form-data               |             | Form data                        |
| text/css                          | .css        | Cascading Style Sheets (CSS)     |
| text/csv                          | .csv        | Comma-separated values (CSV)     |
| text/html                         | .html       | HyperText Markup Language (HTML) |
| text/javascript                   | .js         | JavaScript                       |
| text/plain                        | .txt        | Text                             |

### References

- <https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Identifying_resources_on_the_Web>
- <https://stackoverflow.com/questions/176264/what-is-the-difference-between-a-uri-a-url-and-a-urn>
- <https://developer.mozilla.org/en-US/docs/Learn/Common_questions/Web_mechanics/What_is_a_URL>
- <https://blog.postman.com/what-are-http-methods/>
- <https://dev.to/daryllukas/rest-api-resource-uri-naming-guide-36ac>
- <https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type>
- <https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types>

### Other Sections

- Prev: [The HTTP protocol](./01%20RESTful%20APIs%20-%2002%20The%20HTTP%20protocol.md)
- Next: [Status Codes](./01%20RESTful%20APIs%20-%2004%20Status%20Codes.md)
