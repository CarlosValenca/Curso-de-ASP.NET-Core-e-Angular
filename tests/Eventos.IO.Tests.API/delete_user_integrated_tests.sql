delete Organizadores where CPF = 99999999999;
delete Organizadores where CPF = 67926852082;
delete AspNetUsers where email = 'clientedebugdeteste@me.com';
delete AspNetUsers where email = 'marcospaulo@me.com'


delete Organizadores where id = (select id from AspNetUsers where email = 'marcospaulo2@me.com');
delete AspNetUsers where email = 'marcospaulo2@me.com';

select * from Organizadores where lower(nome) like 'marcos%'

delete Enderecos;
delete Eventos;