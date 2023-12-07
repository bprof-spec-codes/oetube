import { QueryDto } from '@proxy/application/dtos';
import { LoadOptions } from 'devextreme/data';
import { Column } from 'devextreme/ui/data_grid';
import { Builder, Cloner } from 'src/app/base-types/builder';
import { Converter, DefaultConverter } from 'src/app/base-types/converter';

export class IdColumnBuilder extends Builder<Column> {
  constructor(defaultAssigner?:Cloner){
    super({
      caption: 'ID',
      visible: true,
      dataField: 'id',
      dataType: 'string',
      allowSorting: false,
      allowFiltering: false,
    },defaultAssigner)
  }
}

export type FilteredColumn = {
  filterSetter: FilterSetter;
} & Column;



export interface FilterSetter {
  converter: Converter;
  findFilterValue(items: any[], operation: string, name: string): any | undefined;
  setFilterValue(options: LoadOptions, query: QueryDto, column: FilteredColumn): QueryDto;
}
export class DefaultFilterSetter implements FilterSetter {
  converter:Converter = new DefaultConverter();
  setFilterValue(options: LoadOptions, query: QueryDto, column: FilteredColumn): QueryDto {
    query[column.name] = this.findFilterValue(
      options.filter,
      column.selectedFilterOperation,
      column.name
    );
    return query;
  }
  findFilterValue(items: Array<any>, operation: string, name: string) {
    if (items?.length >= 2) {
      if (items[0] == name && items[1] == operation) {
        return items[2];
      } else {
        for (let index = 0; index < items.length; index++) {
          const element = items[index];
          if (element instanceof Array) {
            const result = this.findFilterValue(element, operation, name);
            if (result != undefined) {
              if (this.converter != undefined) {
                return this.converter.convert(result);
              } else {
                return result;
              }
            }
          }
        }
      }
    }
    return undefined;
  }
}
export class NameColumnBuilder extends Builder<FilteredColumn> {
  constructor(defaultAssigner?:Cloner){
    super({
      visible: true,
      dataField: 'name',
      dataType: 'string',
      caption: 'Name',
      allowSorting: true,
      allowFiltering: true,
      selectedFilterOperation: 'contains',
      filterValue: '',
      filterOperations: [],
      filterSetter: new DefaultFilterSetter(),
    },defaultAssigner)
  }
}

export class ThumbnailColumnBuilder extends Builder<Column> {
  constructor(defaultAssigner?:Cloner){
    super({    visible: true,
      dataField: 'thumbnailImage',
      dataType: 'string',
      caption: '',
      width: 100,
      allowFiltering: false,
      allowResizing: false,
      allowSorting: false,
      headerCellTemplate: '',
      cellTemplate: 'thumbnailTemplate',},defaultAssigner)

  }
}

export class BetweenFilterSetter extends DefaultFilterSetter {
  setFilterValue(options: LoadOptions<any>, query: QueryDto, column: FilteredColumn): QueryDto {
    query[column.dataField + 'Min'] = this.findFilterValue(options.filter, '>=', column.dataField);
    query[column.dataField + 'Max'] = this.findFilterValue(options.filter, '<', column.dataField);
    return query;
  }
}

export class CreationTimeFilterSetter extends BetweenFilterSetter {
  converter: Converter = {
    convert(value) {
      if (value) {
        return new Date(value).toISOString();
      }
    },
  };
}
export class CreationTimeColumnBuilder extends Builder<FilteredColumn> {
  constructor(defaultAssigner?:Cloner){
    super({
      visible: true,
      dataField: 'creationTime',
      dataType: 'date',
      caption: 'Creation',
      allowSorting: true,
      allowFiltering: true,
      selectedFilterOperation: 'between',
      format: 'yyyy.MM.dd',
      filterOperations: [],
      filterSetter: new CreationTimeFilterSetter(),
    },defaultAssigner)
  }
}
