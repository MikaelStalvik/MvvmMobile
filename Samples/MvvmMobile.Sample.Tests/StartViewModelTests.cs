﻿using System;
using MvvmMobile.Core.Navigation;
using MvvmMobile.Sample.Core.Model;
using MvvmMobile.Sample.Core.ViewModel;
using NSubstitute;
using NUnit.Framework;
using TinyIoC;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace MvvmMobile.Sample.Tests
{
    [TestFixture, Parallelizable]
    public class StartViewModelTests
    {
        private INavigation _navigation;
        private IStartViewModel _subject;
        private TinyIoCContainer _container;


        [SetUp]
        public void SetUp()
        {
            _container = new TinyIoCContainer();
            Resolver.ResetResolver(new TinyResolver(_container));

            _container.Register<IStartViewModel, StartViewModel>();
            _container.Register<IMotorcyclePayload, MotorcyclePayload>();

            _navigation = Substitute.For<INavigation>();
            _container.Register(_navigation);

            _subject = _container.Resolve<IStartViewModel>();
        }

        [Test]
        public void AddMotorcycleShouldOpenEditMotorcycleViewModel()
        {
            // ARRANGE

            // ACT
            _subject.AddMotorcycleCommand.Execute();

            // ASSERT
            _navigation.Received(1).NavigateTo<IEditMotorcycleViewModel>(null, Arg.Any<Action<Guid>>());
        }

        [Test]
        public void EditMotorcycleShouldOpenEditMotorcycleViewModel()
        {
            // ARRANGE
            var mc = new Motorcycle 
            {
                Id = Guid.NewGuid(),
                Brand = "Honda",
                Model = "VFR800",
                Year = 2000
            };

            // ACT
            _subject.EditMotorcycleCommand.Execute(mc);

            // ASSERT
            _navigation.Received(1).NavigateTo<IEditMotorcycleViewModel>(Arg.Any<IMotorcyclePayload>(), Arg.Any<Action<Guid>>());
        }

        [Test]
        public void EditMotorcycleWithNullParameterShouldNotOpenEditMotorcycleViewModel()
        {
            // ARRANGE

            // ACT
            _subject.EditMotorcycleCommand.Execute(null);

            // ASSERT
            _navigation.Received(0).NavigateTo<IEditMotorcycleViewModel>(Arg.Any<IMotorcyclePayload>(), Arg.Any<Action<Guid>>());
        }

        [Test]
        public void DeleteMotorcycleWhenThereAreNoneShouldNotAffectCount()
        {
            // ARRANGE
            _subject.Motorcycles.Clear();

            var mc = new Motorcycle 
            {
                Id = Guid.NewGuid(),
                Brand = "Honda",
                Model = "VFR800",
                Year = 2000
            };

            var numberOfMotorcycles = _subject.Motorcycles.Count;

            // ACT
            _subject.DeleteMotorcycleCommand.Execute(mc);

            // ASSERT
            Assert.IsTrue(numberOfMotorcycles == _subject.Motorcycles.Count);
        }

        [Test]
        public void DeleteMotorcycleWithNullParameterShouldNotAffectCount()
        {
            // ARRANGE
            var numberOfMotorcycles = _subject.Motorcycles.Count;

            // ACT
            _subject.DeleteMotorcycleCommand.Execute(null);

            // ASSERT
            Assert.IsTrue(numberOfMotorcycles == _subject.Motorcycles.Count);
        }

        [Test]
        public void DeleteMotorcycleShouldRemoveTheMotorcycle()
        {
            // ARRANGE
            _subject.Motorcycles.Clear();

            var mc = new Motorcycle 
            {
                Id = Guid.NewGuid(),
                Brand = "Honda",
                Model = "VFR800",
                Year = 2000
            };

            _subject.Motorcycles.Add(mc);

            var numberOfMotorcycles = _subject.Motorcycles.Count;

            // ACT
            _subject.DeleteMotorcycleCommand.Execute(mc);

            // ASSERT
            Assert.IsTrue(numberOfMotorcycles == 1);
            Assert.IsTrue(_subject.Motorcycles.Count == 0);
        }
    }
}