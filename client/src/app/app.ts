import { Component, inject } from '@angular/core';
import { Nav } from "../layout/nav/nav";
import { Router, RouterOutlet } from "@angular/router";
import { NgClass } from "@angular/common";
import { ConfirmDialog } from "../shared/confirm-dialog/confirm-dialog";

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet, ConfirmDialog],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected router = inject(Router);

}

/**
 * angular 20+ style for routing
 * not using due to it not using full screen. sides are in
 *     [class.container]="router.url ! == '/'"
    [class.mx-auto]="router.url ! == '/'"
 */