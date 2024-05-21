# RESTful APIs

## What is a REST API?

### What is an API?

An Abstract Programming Interface is a set of definitions and protocols for building and integrating application software.

Think of it as a contract between a consumer and a producer, or in other words, a client and a server. The client "requests" an operation and the server "responds" with information.

### What is REST?

Representational State Transfer (REST) is a software architecture that imposes conditions on how an API should work.

REST is independent of any underlying protocol and is not necessarily tied to HTTP.

### So what's a REST API?

A REST API is an API that conforms to the design principles of REST.

### REST principles

| Principle                | Description                                                                                                                                                                                     |
| ------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Uniform interface        | Information should be transferred in a standard format, typically JSON.                                                                                                                         |
| Client-server decoupling | The client and server applications must be independent of each other.                                                                                                                           |
| Statelessness            | Every request is independent of each other. Server-side sessions are not allowed since no client information should be stored.                                                                  |
| Cacheability             | Responses can be cached to improve performance, either on the client-side or the server-side.                                                                                                   |
| Layered system           | Sometimes the client and the server don't connect directly to each other. Requests can be forwarded across multiple systems. These layers should be invisible to the client.                    |
| Code on demand           | The only optional principle. The server responds with a code snippet which the client then executes, but NEVER the other way around. Some examples of this are server-side validation and SPAs. |

### REST API examples

- Web APIs
- Data Access libraries: EntityFramework, MongoDB Driver, etc.
- CRUD repository pattern
- CLIs: Docker CLI, Redis CLI, Azure CLI, Kubectl, etc.

### References

- <https://aws.amazon.com/what-is/restful-api/>
- <https://www.ibm.com/topics/rest-apis>
- <https://www.redhat.com/en/topics/api/what-is-a-rest-api>
- <https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design>

### Other Sections

- Next: [The HTTP protocol](./01%20RESTful%20APIs%20-%2002%20The%20HTTP%20protocol.md)
