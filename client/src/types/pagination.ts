export interface Pagination {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export interface PaginatedResult<T> {
    items: T[];
    metaData: Pagination;
}