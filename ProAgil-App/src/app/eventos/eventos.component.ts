import { HttpClient } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';
import { isNull } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  _filterList: string;

  get filterList() : string {
    return this._filterList; 
  }

  set filterList(value: string) {
    this._filterList = value;
    this.eventosFiltrados = this.filterList ? this.filtrarEvento(this.filterList) : this.eventos;
  }

  eventosFiltrados: any = [];
  eventos: any = [];
  imgWidth = 50;
  imgMargin = 2;
  viewImage = false;
  valueCurrency = 26505;
  
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  alterImage()
  {
    this.viewImage = !this.viewImage;
  }

  getEventos()
  {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.eventos = response;
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  filtrarEvento(filtrarPor: string): any
  {
    filtrarPor = filtrarPor.toLocaleLowerCase();

    return this.eventos.filter(
      item => item.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }
}
