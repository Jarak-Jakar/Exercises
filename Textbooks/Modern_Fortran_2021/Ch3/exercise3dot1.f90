program subroutine_example
  implicit none
  integer :: a
  a = 0
  call add(a, 1)
  print *, a
  call add(a, 2)
  print *, a
  contains
  subroutine add(a, b)
    ! Solution to exercise 3.1
    ! Derived from listing 3.13 in the book.
    integer, intent(in out) :: a
    integer, intent(in) :: b
    a = a + b
  end subroutine add
end program subroutine_example


