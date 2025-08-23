import { Injectable } from '@angular/core';
import {
  HttpInterceptor, HttpRequest, HttpHandler, HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const basic = localStorage.getItem('basicCreds'); // MUST start with "Basic "
    const cloned = basic ? req.clone({ setHeaders: { Authorization: basic } }) : req;

    // DEBUG: confirm the interceptor runs (remove after)
    console.log('[AuthInterceptor]', req.method, req.url, 'addedAuth?', !!basic);

    return next.handle(cloned);
  }
}
