import { Component, OnInit } from '@angular/core';
  
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  
  ngOnInit() {
  const switchers = document.querySelectorAll('.switcher');
  
  if(switchers) { // controllo che l'elemento non sia null
    switchers.forEach((item: any) => {

      item.addEventListener('click', () => {

        Array.from(document.querySelectorAll('.switcher')).forEach((item: any) => { 
          if (item.parentElement) { // controllo che il parent non sia null
            item.parentElement.classList.remove('is-active');
          }
        });

      });
    });
  }  
}
}