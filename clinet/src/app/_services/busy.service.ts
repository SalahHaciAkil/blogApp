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
        type: "ball-clip-rotate-pulse",
        size: "large",
        color: "#9926f0",
        bdColor: "rgba(0,0,0, .9)"
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
