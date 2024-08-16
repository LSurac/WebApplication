import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss']
})
export class InputComponent {
  @Input() public inputLabel?: string;
  @Input() public type?: string;
  @Output() confirmInput: EventEmitter<string> = new EventEmitter()
  public value: string | undefined;

  confirm(event: KeyboardEvent) {
    if (event.key !== "Enter") {
      return;
    }

    this.emit()
  }

  emit() {
    this.value = this.value?.trim();

    this.confirmInput.emit(this.value)
  }
}
