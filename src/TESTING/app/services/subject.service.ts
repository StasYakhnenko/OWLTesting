import { Injectable} from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import { Subject } from '../components/subject';
import 'rxjs/add/operator/map'
import 'rxjs/Rx';

@Injectable()
export class SubjectService {
    public headers: Headers;

    constructor(private http: Http) {
        this.headers = new Headers({ 'Content-Type': 'application/json' });
    }

    getSubjects() {
        return this.http.get('api/subjects/all').map(this.extractData);
    }

    private extractData(res: Response) {
        let body = res.json();
        console.log(body);
        return body;
    }

    addSubject(subject: Subject) {

        return this.http.post('api/subjects/add', JSON.stringify(subject), { headers: this.headers })
            .map((res: Response) => res.json());
    }

    deleteSubject(subject: Subject) {

        return this.http.post('api/subjects/delete', JSON.stringify(subject), { headers: this.headers })
            .map((res: Response) => res.json());
    }

    updateSubject(subject: Subject) {
        console.log(subject);
        return this.http.post('api/subjects/update', JSON.stringify(subject), { headers: this.headers })
            .map((res: Response) => res.json());
    }

}