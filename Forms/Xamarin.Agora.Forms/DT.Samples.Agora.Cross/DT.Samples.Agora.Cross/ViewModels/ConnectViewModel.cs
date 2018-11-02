using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using DT.Samples.Agora.Cross.Models;
using DT.Samples.Agora.Cross.Views;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace DT.Samples.Agora.Cross.ViewModels
{
    public class ConnectViewModel : BaseViewModel
    {
        private string _roomName;

        public string RoomName
        {
            get
            {
                return _roomName;
            }
            set
            {
                SetProperty(ref _roomName, value);
            }
        }

        public Command LoadItemsCommand { get; set; }

        public ConnectViewModel()
        {
            Title = "Join Room";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CheckPermissionsAndStart();
        }

        private async void CheckPermissionsAndStart()
        {
            var permissionsToRequest = new List<Permission>();
            var cameraPermissionState = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraPermissionState != PermissionStatus.Granted)
                permissionsToRequest.Add(Permission.Camera);

            var microphonePermissionState = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (microphonePermissionState != PermissionStatus.Granted)
                permissionsToRequest.Add(Permission.Microphone);

            var storagePermissionState = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storagePermissionState != PermissionStatus.Granted)
                permissionsToRequest.Add(Permission.Storage);

            if (permissionsToRequest.Count > 0)
                await CrossPermissions.Current.RequestPermissionsAsync(permissionsToRequest.ToArray());
        }

        async Task ExecuteLoadItemsCommand()
        {
        }
    }
}