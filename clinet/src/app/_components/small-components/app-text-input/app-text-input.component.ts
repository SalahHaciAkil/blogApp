import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './app-text-input.component.html',
  styleUrls: ['./app-text-input.component.scss']
})
export class AppTextInputComponent implements ControlValueAccessor {

  @Input() type: string;
  @Input() label: string;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }


  ngOnInit(): void {
  }

}
