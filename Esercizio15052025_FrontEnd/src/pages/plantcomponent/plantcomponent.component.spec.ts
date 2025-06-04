import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantcomponentComponent } from './plantcomponent.component';

describe('PlantcomponentComponent', () => {
  let component: PlantcomponentComponent;
  let fixture: ComponentFixture<PlantcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlantcomponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
