# Search Requests

## Pagination

### What is API pagination?

It's a technique used to retrieve large data sets in a manageable manner. When an endpoint returns large amounts of data, pagination allows the data to be divided into smaller "pages", each with a limited number of results.

**API pagination benefits:**

- Improved performance
- Reduced resource usage
- Enhanced user experience
- Efficient data transfer
- Scalability and flexibility
- Error handling

### Common API pagination techniques

```text
# Offset Limit pagination

GET /api/posts?offset=0&limit=10
```

```text
# Page-based pagination

GET /api/posts?page=2&size=20
```

**Other less common techniques are:**

```text
# Cursor pagination

GET /api/posts?cursor=eyJpZCI6MX0
```

```text
# Time-based pagination

GET /api/events?start_time=2023-01-01T00:00:00Z&end_time=2023-01-31T23:59:59Z
```

```text
# Keyset pagination

GET /api/products?last_key=XYZ123
```

### Pagination Metadata

Always include pagination metadata in the API response:

- The current page
- The current size
- The next page
- The previous page
- The total number of pages
- The total number of records

This metadata helps API consumers navigate through the paginated data more effectively.

Example:

```json
{
  "results": [
    {
      "id": 1,
      "title": "Post 1",
      "content": "Lorem ipsum dolor sit amet."
    },
    ...
  ],
  "pagination": {
    "currentPage": 3,
    "currentSize": 10,
    "nextPage": 4,
    "prevPage": 2,
    "totalPages": 10,
    "totalCount": 100
  }
}
```

## Sorting

Examples:

```text
GET /api/posts?sort=displayName
GET /api/posts?sort=displayName asc
GET /api/posts?sort=displayName desc

GET /api/posts?order=displayName
GET /api/posts?order=displayName asc
GET /api/posts?order=displayName desc

GET /api/posts?sortBy=displayName&sortOrder=asc

GET /api/posts?orderBy=displayName&orderDir=asc
```

## Filtering

Examples:

```text
GET /api/posts?q=funny cats

GET /api/posts?query=funny cats

GET /api/posts?filter=funny cats

GET /api/posts?search=funny cats

GET /api/posts?term=funny cats
```

## Real-world usage

**Real-world uses of pagination:**

- LinkedIn uses pagination to retrieve posts and comments.
- Amazon uses pagination to retrieve product listings, search results and user reviews.
- Banking apps use pagination to retrieve transaction history.

**Real-world APIs that use pagination:**

- <https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#list-repositories-for-a-user>
- <https://www.elastic.co/guide/en/elasticsearch/reference/current/search-search.html#search-search-api-example>
- <https://pokeapi.co/docs/v2#resource-listspagination-section>
- <https://nominatim.org/>

## References

- <https://dev.to/pragativerma18/unlocking-the-power-of-api-pagination-best-practices-and-strategies-4b49>
