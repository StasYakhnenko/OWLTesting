import { Component } from "@angular/core";
import { Subject } from "./subject"
import { SubjectService } from "../services/subject.service"

@Component({
    moduleId: module.id,
    selector: "subject",
    templateUrl: "../../templates/subject/subject.template.html",
    styleUrls: ["../../css/subject/subject.css"],
    providers: [SubjectService]
})
export class SubjectComponent {
    subjects: Subject[];
    subjectToUpdate: Subject;

    constructor(private subjectService: SubjectService) {
        //this.subjects.push({
        //    Id: 1,
        //    Name: ".NET",
        //    Describtion:"The .NET framework helps you create mobile, desktop, and web applications that run on Windows PCs, devices and servers and it's included in Visual Studio"
        //});
        //this.subjects.push({
        //    Id: 2,
        //    Name: "Algorithms",
        //    Describtion: "Algorithms are the heart of computer science, and the subject has countless practical applications as well as intellectual depth. This specialization is an introduction to algorithms for learners with at least a little programming experience"
        //});
        console.log(this.subjects);

        this.subjectService.getSubjects().subscribe(subjects => { this.subjects = subjects });
        console.log(this.subjects);
    }

    addSubject(name: string, desc: string) {
        this.subjects.push({id: 0, name: name, describtion: desc });
        this.subjectService.addSubject({ id: 0, name: name, describtion: desc }).subscribe(res => { console.log(res); });
    }

    deleteSubject(subject: Subject) {
        let i: number = this.subjects.indexOf(subject);
        this.subjects.splice(i, 1);
        this.subjectService.deleteSubject(subject).subscribe(res => { console.log(res) });
    }

    updateSubject(subject: Subject) {
        this.subjectToUpdate = subject;
    }

    UpdateSubjectSubmit(name: string, desc: string, id: number) {
        let subject: Subject = this.subjects.filter((el) => el.id == id)[0];
        subject.name = name;
        subject.describtion = desc;
        this.subjectService.updateSubject(subject).subscribe(res => { console.log(res) });

        this.subjectToUpdate = null;
    }
    CancelUpdatingSubjects() {
        this.subjectToUpdate = null;
    }
}