
export interface PaginationDto<T> {
  items: T[];
  currentPage: number;
  pageCount: number;
  totalCount: number;
  count: number;
}
