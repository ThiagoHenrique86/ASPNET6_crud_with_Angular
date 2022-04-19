
import { Component, OnInit } from '@angular/core';
import { Card } from './Models/card.model';
import { CardsService } from './Service/cards.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'cards';
  cards: Card[] = [];
  card: Card = {
    id: '',
    cardNumber: '',
    cardholderName: '',
    expiryMonth: 0,
    expiryYear: 0,
    cvc: 0
  };

  limpaCampos(){
    this.card = {
      id: '',
      cardNumber: '',
      cardholderName: '',
      expiryMonth: 0,
      expiryYear: 0,
      cvc: 0
    };
  }

  constructor(private cardsService: CardsService){}


  ngOnInit(): void {
    this.getAllCards();
  }


  getAllCards(){
    this.cardsService.getAllCards()
    .subscribe(
      response => {
        //console.log(JSON.stringify(response));
        this.cards = response;
        this.limpaCampos();
      }
    );
  }

  onSubmit(){

    if(this.card.id === ''){

      this.cardsService.addCard(this.card)
      .subscribe(
        response => {
          this.getAllCards();
        }
      )

    }else{
      this.updateCard(this.card)
    }



  }


  deleteCard(id: string){
    this.cardsService.deleteCard(id)
    .subscribe(
      response => {
        this.getAllCards();
      }
    );
  }

  populateForm(card: Card){
    this.card = card;
  }

  updateCard(card: Card){
    this.cardsService.updateCard(card)
    .subscribe(
      response => {
        this.getAllCards();
      }
    );
  }





}
