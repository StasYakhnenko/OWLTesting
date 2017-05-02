"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var subject_service_1 = require("../services/subject.service");
var SubjectComponent = (function () {
    function SubjectComponent(subjectService) {
        var _this = this;
        this.subjectService = subjectService;
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
        this.subjectService.getSubjects().subscribe(function (subjects) { _this.subjects = subjects; });
        console.log(this.subjects);
    }
    SubjectComponent.prototype.addSubject = function (name, desc) {
        this.subjects.push({ id: 0, name: name, describtion: desc });
        this.subjectService.addSubject({ id: 0, name: name, describtion: desc }).subscribe(function (res) { console.log(res); });
    };
    SubjectComponent.prototype.deleteSubject = function (subject) {
        var i = this.subjects.indexOf(subject);
        this.subjects.splice(i, 1);
        this.subjectService.deleteSubject(subject).subscribe(function (res) { console.log(res); });
    };
    SubjectComponent.prototype.updateSubject = function (subject) {
        this.subjectToUpdate = subject;
    };
    SubjectComponent.prototype.UpdateSubjectSubmit = function (name, desc, id) {
        var subject = this.subjects.filter(function (el) { return el.id == id; })[0];
        subject.name = name;
        subject.describtion = desc;
        this.subjectService.updateSubject(subject).subscribe(function (res) { console.log(res); });
        this.subjectToUpdate = null;
    };
    SubjectComponent.prototype.CancelUpdatingSubjects = function () {
        this.subjectToUpdate = null;
    };
    SubjectComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: "subject",
            templateUrl: "../../templates/subject/subject.template.html",
            styleUrls: ["../../css/subject/subject.css"],
            providers: [subject_service_1.SubjectService]
        }), 
        __metadata('design:paramtypes', [subject_service_1.SubjectService])
    ], SubjectComponent);
    return SubjectComponent;
}());
exports.SubjectComponent = SubjectComponent;
//# sourceMappingURL=subject.component.js.map