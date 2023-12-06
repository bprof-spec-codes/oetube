export interface Converter<T=any, K=any> {
    convert: (value: T) => K;
    convertBack?: (value: K) => T;
  }
export class DefaultConverter implements Converter{
  convert(v:any){
    return v
  }
  convertBack(v:any){
    return v
  }
}
