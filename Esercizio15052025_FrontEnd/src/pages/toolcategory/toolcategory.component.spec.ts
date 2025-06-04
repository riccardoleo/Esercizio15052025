import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolcategoryComponent } from './toolcategory.component';

describe('ToolcategoryComponent', () => {
  let component: ToolcategoryComponent;
  let fixture: ComponentFixture<ToolcategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToolcategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToolcategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
