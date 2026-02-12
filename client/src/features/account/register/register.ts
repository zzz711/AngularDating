import { Component, inject, output } from '@angular/core';
import { RegisterCreds } from '../../../types/user';
import { FormsModule } from "@angular/forms";
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;
  private accountService = inject(AccountService);

  register() {
    this.accountService.register(this.creds).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error : error => console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
