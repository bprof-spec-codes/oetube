import { Component, Input,Output,EventEmitter } from '@angular/core';

@Component({
  selector: 'app-delete-dialog-button',
  templateUrl: './delete-dialog-button.component.html',
  styleUrls: ['./delete-dialog-button.component.scss'],
})
export class DeleteDialogButtonComponent {
  @Input() visible:boolean
  popupVisible:boolean
  @Output() confirmed:EventEmitter<any>=new EventEmitter()
  cancel(){
    this.popupVisible=false
  }
  confirm(){
    this.confirmed.emit()
  }
  onClick(){
    this.popupVisible=true
  }
}
