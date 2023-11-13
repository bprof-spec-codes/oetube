import type { ItemPerPage } from '../../domain/repositories/query-args/item-per-page.enum';

export interface QueryDto {
  itemPerPage: ItemPerPage;
  page: number;
  sorting?: string;
}

export interface PaginationDto<T> {
  items: T[];
  currentPage: number;
  pageCount: number;
  totalCount: number;
  count: number;
}
