import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router){}
     token = ""
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.token = localStorage.getItem('token')
        if (this.token != null) {
            const clonedRequest = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${this.token}`)
            });
            return next.handle(clonedRequest).pipe(
                tap(
                    success => {

                    },
                    error => {
                        if(error.status == 401){
                            localStorage.removeItem('token')
                        this.router.navigateByUrl('/accounts/signin');
                        }
                    }
                )
            )
        }
        return next.handle(req.clone());
    }

}