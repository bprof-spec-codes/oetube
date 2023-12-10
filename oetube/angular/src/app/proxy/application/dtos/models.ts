import type { Pagination } from '../../domain/repositories/query-args/models';

export interface QueryDto {
  pagination: Pagination;
  sorting?: string;
}

export interface PaginationDto<T> {
  items: T[];
  skip: number;
  take: number;
  totalCount: number;
  count: number;
}
