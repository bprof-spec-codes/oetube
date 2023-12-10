
export type TimeInitType=Time|{hours?:number,minutes?:number,seconds?:number}|string|number
export class Time{
    private _totalSeconds:number
    get totalSeconds(){
      return this._totalSeconds
    }
    get totalMinutes(){
      return this._totalSeconds/60
    }
    get totalHours(){
      return this.totalMinutes/24
    }
    get hours(){
      return parseInt((this._totalSeconds/60/60).toString())
    }
    get minutes(){
      return parseInt((this._totalSeconds/60%60).toString())
    }
    get seconds(){
      return this._totalSeconds%60
    }
 
    toString(sFractions:boolean=false){
      const padLength=2
      const zero='0'
      const seconds=sFractions?this.seconds:Number.parseInt(this.seconds.toString())
      const parts=[this.hours.toString().padStart(padLength,zero),
                  this.minutes.toString().padStart(padLength,zero),
                  seconds.toString().padStart(padLength,zero)
                ]
      return parts.join(":")
    }
    static readonly zero:Time=new Time({})
    
    static add(x:Time,y:Time){
        return Time.fromNumber(x._totalSeconds-y._totalSeconds)
    }
    static subtract(x:Time,y:Time){
        return Time.fromNumber(x._totalSeconds-y._totalSeconds)
    }

    constructor (time:{hours?:number,minutes?:number,seconds?:number}){
        time??={}
        time.hours??=0
        time.minutes??=0
        time.seconds??=0
        this._totalSeconds=time.hours*60*60+time.minutes*60+time.seconds
    }
    static from(time:TimeInitType){
        if(!time){
            return Time.zero
        }
        if(typeof time=="string"){
            return Time.parseTime(time)
        }
        if(typeof time=="number"){
            return Time.fromNumber(time)
        }
        if(time instanceof Time){
            return time
        }

        return new Time(time)
    }

    static fromNumber(number:number){
        return new Time({seconds:number})
    }

    static parseTime(string:string){
      if(!string){
        return 
      }
      const parts=string.split(":")
      if(parts.length!=3){ return }

      const hours=Number.parseInt(parts[0])
      if(Number.isNaN(hours)){
        return
      }
      const minutes=Number.parseInt(parts[1])
      if(Number.isNaN(minutes)){
        return
      }
      const seconds=Number.parseFloat(parts[2])
      if(Number.isNaN(seconds)){
        return
      }
      return new Time({hours:hours,minutes:minutes,seconds:seconds})
    }
  }