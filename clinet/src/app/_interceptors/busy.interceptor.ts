import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators';
import { BusyService } from '../_services/busy.service';

@Injectable()
export class BusyInterceptor implements HttpInterceptor {

  constructor(private spinner: BusyService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log(request);

    this.spinner.play();
    return next.handle(request).pipe(
      // delay(1000),
      finalize(() => {
        console.log("after", request);
        this.spinner.hide();
      })
    );
  }
}
