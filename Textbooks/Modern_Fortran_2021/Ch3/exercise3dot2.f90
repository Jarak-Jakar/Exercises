program cold_front
  ! This program calculates temperature due to cold front
  ! passage for different times using an external function.
  ! A solution to exercise 3.2, derived from the supplied listing 3.3 in the book.
  implicit none

  real :: dt(8)

  dt = [6, 12, 18, 24, 30, 36, 42, 48]

    ! print *, 'Temparature after ', nhours, ' hours is ', & 
    !   cold_front_temperature(12., 24., 20., 960., nhours), &
    !   ' degrees.'

  print *, cold_front_temperature(12., 24., 20., 960., dt)

contains

  real pure elemental function cold_front_temperature(temp1, temp2, c, dx, dt) result(r)
    ! Returns the temperature after dt hours, given initial 
    ! temperatures at origin (temp1) and destination (temp2),
    ! front speed (c), and distance between the two locations (dx).
    real, intent(in) :: temp1, temp2, c, dx, dt
    r = temp2 - c * (temp2 - temp1) / dx * dt
  end function cold_front_temperature

end program cold_front
