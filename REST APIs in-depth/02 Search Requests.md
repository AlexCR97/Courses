# Search Requests

> **_Status Codes exercise!_**

## Pagination

### What is API pagination?

API Pagination is a technique used in API design to retrieve large data sets in a manageable manner. When an endpoint returns a large amount of data, pagination allows the data to be divided into smaller chunks or "pages". Each page contains a limited number of records.

**API pagination benefits:**

- Improved performance
- Reduced resource usage
- Enhanced user experience
- Efficient data transfer
- Scalability and flexibility
- Error handling

**Real-world uses of pagination:**

- LinkedIn uses pagination to retrieve posts and comments.
- Amazon uses pagination to retrieve product listings, search results and user reviews.
- Banking apps use pagination to retrieve transaction history.
- Job search platforms like Indeed or LinkedIn Jobs offer paginated APIs for retrieving job listings based on various criteria such as location, industry, or keywords.

**Real-world APIs that use search requests:**

- <https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#list-repositories-for-a-user>
- <https://www.elastic.co/guide/en/elasticsearch/reference/current/search-search.html#search-search-api-example>
- <https://developers.google.com/photos/library/reference/rest/v1/albums/list>
- <https://pokeapi.co/docs/v2#resource-listspagination-section>
- <https://nominatim.org/>

### Common API pagination techniques

```text
# Offset Limit pagination
GET /api/posts?offset=0&limit=10

# Page-based pagination
GET /api/posts?page=2&size=20

# Cursor pagination
GET /api/posts?cursor=eyJpZCI6MX0

# Time-based pagination
GET /api/events?start_time=2023-01-01T00:00:00Z&end_time=2023-01-31T23:59:59Z

# Keyset pagination
GET /api/products?last_key=XYZ123
```

### Pagination Metadata

Always include pagination metadata in the API response:

- Total number of records
- The current page
- The number of pages
- Links to the next and previous pages

This metadata helps API consumers navigate through the paginated data more effectively.

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
    "totalCount": 100,
    "currentPage": 1,
    "totalPages": 10,
    "nextPage": 2,
    "prevPage": null
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

## References

- <https://dev.to/pragativerma18/unlocking-the-power-of-api-pagination-best-practices-and-strategies-4b49>
