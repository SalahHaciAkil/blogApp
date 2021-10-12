import { Injectable } from '@angular/core';
import { NgxSpinnerService } from "ngx-spinner";

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  requests: number = 0;


  constructor(private spinnerService: NgxSpinnerService) {

  }


  play() {
    this.requests++;
    if (this.requests == 1) {
      this.spinnerService.show(undefined, {
        type: "ball-atom",
        size: "large",
        color: "#000000",
        bdColor: "rgba(0,0,0, 0.0)"
      });
    }

  }


  hide() {
    this.requests--;
    if (this.requests <= 0) {
      this.requests = 0;
      this.spinnerService.hide();
    }

  }
}
