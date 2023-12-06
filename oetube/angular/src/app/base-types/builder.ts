

export interface Cloner {
  Clone<T=any>(obj: T): T;
}

export class Assigner implements Cloner {
  Clone<T=any>(obj: T): T {
    return obj;
  }
}
export class ShallowCloner implements Cloner {
  Clone<T=any>(obj: T): T {
    if (obj == undefined) {
      return obj;
    }
    return Object.assign({}, obj);
  }
}

export class DeepCloner implements Cloner {
  Clone<T=any>(obj: T): T {
    if (obj == undefined) {
      return obj;
    }
    if (obj instanceof Array) {
      const clone = [];
      obj.forEach(i => {
        clone.push(this.Clone(i));
      });
      return clone as T;
    } else if (obj instanceof Object) {
      const clone = {};
      Object.keys(obj).forEach(k => {
        clone[k] = this.Clone(obj[k]);
      });
      return clone as T;
    } else {
      const clone = obj;
      return clone as T;
    }
  }
}

export class Builder<T>{
  prototype:T
  defaultAssigner:Cloner
  constructor(prototype:T, defaultAssigner?:Cloner){
    this.defaultAssigner=defaultAssigner??new Assigner()
    this.prototype=this.defaultAssigner.Clone(prototype)
  }
  setDefaultAssigner(assigner:Cloner){
    this.defaultAssigner=assigner
  }
  map(source:Partial<T>, assigner?:Cloner){
    if (source == undefined) {
      return this;
    }
    assigner??=this.defaultAssigner;
    Object.keys(source).forEach(k=>{
      this.prototype[k]=assigner.Clone(source[k])
    })
    return this
  }

  transformDescendant<K extends T>(descendant:K, assigner?:Cloner){
    assigner??=this.defaultAssigner
    const builder=new Builder<K>(descendant,assigner)
    builder.map(this.prototype as Partial<K>,assigner)
    return builder
  }
  transformAncestor<K extends Partial<T>>(ancestor:K,assigner?:Cloner){
    assigner??=this.defaultAssigner
    const builder=new Builder<K>(ancestor,assigner)
    builder.map(this.prototype as Partial<K>,assigner)
    return builder
  }
  build(assigner?:Cloner){
    assigner??=this.defaultAssigner
    return assigner.Clone(this.prototype)
  }
}
export class ObjectPropertyChanger<T extends Object>{
  object:T
  constructor(obj:T){
     this.object=obj
  }
  changeProperty<K extends keyof T>(key:Extract<K,string>, source:Partial<T[K]>, assigner?:Cloner){
    const property=this.object[key]
    const builder=new Builder<T[K]>(property,assigner)
    this.object[key as string]=builder.map(source).build()
  } 
}