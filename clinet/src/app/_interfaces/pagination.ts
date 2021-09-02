export class PaginationResult<T> {
    pagination: Pagination;
    result: T;

}

export interface Pagination {
    currentPage: number,
    itemsPerPage: number,
    totalItems: number,
    totalPages: number
};