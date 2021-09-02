import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginationResult } from "../_interfaces/pagination";

export function getPaginationHeaders(pageNumber:number, pageSize:number){
    let httpParams:HttpParams = new HttpParams();

    httpParams = httpParams.append('pageNumber', pageNumber);
    httpParams = httpParams.append('pageSize', pageSize);
    debugger;

    return httpParams;
}

export function getPaginationResult<T>(http:HttpClient, url:string, httpParams:HttpParams)
{
    const paginatedResult:PaginationResult<T> = new PaginationResult<T>();
    return http.get(url, { observe: 'response', params: httpParams }).pipe(
        map((respone) =>{
            paginatedResult.result = <T>respone.body;
            let paginationHeader = respone.headers.get("Pagination")
            if( paginationHeader != null){
                paginatedResult.pagination = JSON.parse(paginationHeader);
            }
            debugger;
            return paginatedResult;
        })
    )

}