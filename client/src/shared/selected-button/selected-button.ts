import { Component, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-selected-button',
  imports: [],
  templateUrl: './selected-button.html',
  styleUrl: './selected-button.css'
})
export class SelectedButton {
  disabled = input<boolean>(true);
  selected = input<boolean>(false);
  clickEvent = output<Event>();

  onClick(event: Event) {
    this.clickEvent.emit(event);
  }
}